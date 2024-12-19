import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:evaluate_app/models/models.dart';
import 'package:intl/intl.dart';
import 'package:evaluate_app/data/data_provider.dart';

final storage = const FlutterSecureStorage();

Future<Map<String, dynamic>> fetchEvaluationCriteria() async {
  final token = await storage.read(key: 'accessToken');
  final url = Uri.parse(AppConfig.criteriaAndItems);

  try {
    final response = await http.get(
      url,
      headers: {
        'Authorization': 'Bearer $token',
      },
    );
    if (response.statusCode == 200) {
      print('${response.statusCode}: Criterias fetched successfully!');
      final Map<String, dynamic> data = json.decode(response.body);
      return data;
    } else {
      throw Exception('${response.statusCode}: Failed to load criteria.');
    }
  } catch (error) {
    print('Error fetching evaluation criteria: $error');
    return {};
  }
}

class EvaluateProjectPage extends StatefulWidget {
  final DataProvider dataProvider = DataProvider();
  final Project project;

  EvaluateProjectPage({Key? key, required this.project}) : super(key: key);

  @override
  _EvaluateProjectPageState createState() => _EvaluateProjectPageState();
}

class _EvaluateProjectPageState extends State<EvaluateProjectPage> {
  final List<TextEditingController> _criteriaControllers = [];
  final List<TextEditingController> _criteriaFeedbackControllers = [];
  final List<TextEditingController> _checklistFeedbackControllers = [];
  final TextEditingController _generalFeedbackController =
      TextEditingController();
  int totalScore = 0;
  late List<bool> _checklistCheckboxes;
  late List<bool> _criteriaCheckboxes;

  @override
  void initState() {
    super.initState();
    fetchEvaluationCriteria().then((data) {
      final criteria = data['evaluationCriteriaDatas'] ?? [];
      final checklist = data['checklistItemDatas'] ?? [];

      setState(() {
        _criteriaControllers.addAll(
            List.generate(criteria.length, (index) => TextEditingController()));
        _criteriaFeedbackControllers.addAll(
            List.generate(criteria.length, (index) => TextEditingController()));
        _checklistFeedbackControllers.addAll(List.generate(
            checklist.length, (index) => TextEditingController()));
        _checklistCheckboxes =
            List.generate(checklist.length, (index) => false);
        _criteriaCheckboxes = List.generate(criteria.length, (index) => true);
      });
    });
  }

  @override
  void dispose() {
    for (var controller in _criteriaControllers) {
      controller.dispose();
    }
    for (var controller in _criteriaFeedbackControllers) {
      controller.dispose();
    }
    for (var controller in _checklistFeedbackControllers) {
      controller.dispose();
    }
    _generalFeedbackController.dispose();
    super.dispose();
  }

  Future<void> submitEvaluation() async {
    final token = await storage.read(key: 'accessToken');
    final url = Uri.parse(AppConfig.submitEvaluation);
    final now = DateTime.now().toUtc();

    final formattedDate = DateFormat("yyyy-MM-ddTHH:mm:ss.SSS'Z'").format(now);

    final evaluationData = {
      "teamId": widget.project.teamId,
      "generalComments": _generalFeedbackController.text,
      "totalScore": totalScore,
      "date": formattedDate,
      "evaluationCriterias":
          List.generate(_criteriaControllers.length, (index) {
        return {
          "criteriaId": index + 1,
          "isChecked": _criteriaCheckboxes[index],
          "score": int.tryParse(_criteriaControllers[index].text) ?? 0,
          "feedback": _criteriaFeedbackControllers[index].text,
        };
      }),
      "evaluationChecklistItems":
          List.generate(_checklistFeedbackControllers.length, (index) {
        return {
          "itemId": index + 1,
          "isChecked": _checklistCheckboxes[index],
          "feedback": _checklistFeedbackControllers[index].text,
        };
      }),
    };

    try {
      final response = await http.post(
        url,
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
        body: json.encode(evaluationData),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        print('${response.statusCode}: Evaluation submitted successfully!');
        Navigator.of(context).pop();
      } else {
        throw Exception('Failed to submit evaluation: ${response.body}');
      }
    } catch (error) {
      print('Error submitting evaluation: $error');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        scrolledUnderElevation: 0,
        backgroundColor: AppColors.primary,
        elevation: 0,
        centerTitle: false,
        titleTextStyle: const TextStyle(
          color: AppColors.whiteTextColor,
          fontFamily: 'Inter',
          fontSize: 24,
          fontWeight: FontWeight.bold,
        ),
        title: Text(widget.project.teamName),
        leading: IconButton(
          color: AppColors.whiteTextColor,
          icon: Icon(Icons.arrow_back),
          onPressed: () => Navigator.of(context).pop(),
        ),
        toolbarHeight: 60,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.vertical(
            bottom: Radius.circular(20),
          ),
        ),
      ),
      body: FutureBuilder<Map<String, dynamic>>(
        future: fetchEvaluationCriteria(),
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(child: Text("Error: ${snapshot.error}"));
          } else if (!snapshot.hasData || snapshot.data == null) {
            return const Center(child: Text("No data available"));
          }

          final data = snapshot.data!;
          final evaluationCriteria =
              data['evaluationCriteriaDatas'] as List<dynamic>? ?? [];
          final checklistItems =
              data['checklistItemDatas'] as List<dynamic>? ?? [];

          if (evaluationCriteria.isEmpty && checklistItems.isEmpty) {
            return const Center(
                child: Text("No criteria or checklist items found"));
          }

          return SingleChildScrollView(
            child: Container(
              decoration: BoxDecoration(
                color: AppColors.pageBackground,
              ),
              padding: const EdgeInsets.fromLTRB(13.0, 13.0, 13.0, 13.0),
              child: Container(
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(30),
                  border: Border.all(
                    color: const Color.fromARGB(255, 201, 201, 201),
                    width: 0.7,
                  ),
                ),
                child: Padding(
                  padding: const EdgeInsets.all(13.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        widget.project.projectName,
                        style: const TextStyle(
                            fontSize: 18, fontWeight: FontWeight.bold),
                      ),
                      const SizedBox(height: 10),
                      const Text("Project Description",
                          style: TextStyle(fontWeight: FontWeight.bold)),
                      Text(widget.project.description),
                      const SizedBox(height: 10),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Text("Evaluating Teacher",
                                  style:
                                      TextStyle(fontWeight: FontWeight.bold)),
                              Text(widget.project.evaluatingTeacherFullName),
                            ],
                          ),
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Text("Status",
                                  style:
                                      TextStyle(fontWeight: FontWeight.bold)),
                              Row(
                                children: const [
                                  Icon(Icons.circle,
                                      size: 8, color: Color(0xFF00B7FF)),
                                  SizedBox(width: 4),
                                  Text(
                                    "Ready to Evaluate",
                                    style: TextStyle(
                                        color: Color(0xFF00B7FF),
                                        fontWeight: FontWeight.bold),
                                  ),
                                ],
                              ),
                            ],
                          ),
                        ],
                      ),
                      const SizedBox(height: 14),
                      const Text("Team",
                          style: TextStyle(fontWeight: FontWeight.bold)),
                      Text(widget.project.teamName),
                      const SizedBox(height: 14),
                      const Text("Team Members",
                          style: TextStyle(fontWeight: FontWeight.bold)),
                      Text("Alparslan Eravsar - 2000003498"),
                      Text("Semir Kimyonsen - 2000004562"),
                      Text("Onur Taha Çetinkaya - 2000003710"),
                      const Divider(height: 20, thickness: 1),
                      const Text(
                        "Part I - Evaluation Project Graduation Form",
                        style: TextStyle(
                            fontWeight: FontWeight.bold, fontSize: 16),
                      ),
                      const SizedBox(height: 14),
                      ...evaluationCriteria.map<Widget>((criteria) {
                        final index = evaluationCriteria.indexOf(criteria);
                        return Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Expanded(
                                  child: Text(
                                    criteria['criteriaName'],
                                    style: const TextStyle(fontSize: 16),
                                  ),
                                ),
                                const SizedBox(
                                  width: 60,
                                  height: 40,
                                  child: TextField(
                                    decoration: InputDecoration(
                                      border: OutlineInputBorder(
                                        borderRadius: BorderRadius.all(
                                            Radius.circular(10.0)),
                                      ),
                                      focusedBorder: OutlineInputBorder(
                                        borderRadius: BorderRadius.all(
                                            Radius.circular(10.0)),
                                        borderSide: BorderSide(
                                            color: Color(0xFF00B7FF),
                                            width: 2.0),
                                      ),
                                    ),
                                    keyboardType: TextInputType.number,
                                  ),
                                ),
                              ],
                            ),
                            const SizedBox(height: 8),
                            const TextField(
                              decoration: InputDecoration(
                                hintText: "Write your thoughts...",
                                border: OutlineInputBorder(
                                  borderRadius:
                                      BorderRadius.all(Radius.circular(10.0)),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderRadius:
                                      BorderRadius.all(Radius.circular(10.0)),
                                  borderSide: BorderSide(
                                      color: Color(0xFF00B7FF), width: 2.0),
                                ),
                              ),
                              maxLines: 3,
                            ),
                            const SizedBox(height: 14),
                          ],
                        );
                      }).toList(),
                      const Divider(height: 20, thickness: 1),
                      const Text(
                        "Part II - Graduation Project Checklist",
                        style: TextStyle(
                            fontWeight: FontWeight.bold, fontSize: 16),
                      ),
                      ...checklistItems.map<Widget>((item) {
                        final index = checklistItems.indexOf(item);
                        return Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Expanded(
                                  child: Text(
                                    item['itemName'],
                                    style: const TextStyle(fontSize: 16),
                                  ),
                                ),
                              ],
                            ),
                            const SizedBox(height: 8),
                            const TextField(
                              decoration: InputDecoration(
                                hintText: "Write your thoughts...",
                                border: OutlineInputBorder(
                                  borderRadius:
                                      BorderRadius.all(Radius.circular(10.0)),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderRadius:
                                      BorderRadius.all(Radius.circular(10.0)),
                                  borderSide: BorderSide(
                                      color: Color(0xFF00B7FF), width: 2.0),
                                ),
                              ),
                              maxLines: 3,
                            ),
                            const SizedBox(height: 14),
                          ],
                        );
                      }).toList(),
                      Divider(height: 20, thickness: 1),
                      SizedBox(height: 10),
                      const Text("General Feedback",
                          style: TextStyle(fontWeight: FontWeight.bold)),
                      SizedBox(height: 10),
                      TextField(
                        decoration: const InputDecoration(
                          hintText: "Write your general feedback...",
                          border: OutlineInputBorder(
                            borderRadius:
                                BorderRadius.all(Radius.circular(10.0)),
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderRadius:
                                BorderRadius.all(Radius.circular(10.0)),
                            borderSide: BorderSide(
                                color: Color(0xFF00B7FF), width: 2.0),
                          ),
                        ),
                        maxLines: 6,
                      ),
                      SizedBox(height: 20),
                      ElevatedButton(
                        onPressed: () async {
                          await submitEvaluation();
                        },
                        style: ElevatedButton.styleFrom(
                          backgroundColor: AppColors.primary,
                          minimumSize: Size(double.infinity, 50),
                        ),
                        child: Text(
                          "Confirm",
                          style: TextStyle(
                              fontWeight: FontWeight.bold,
                              color: AppColors.whiteTextColor),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          );
        },
      ),
    );
  }
}
