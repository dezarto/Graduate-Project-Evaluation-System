import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:evaluate_app/models/models.dart';

final storage = const FlutterSecureStorage();

Future<Map<String, dynamic>> fetchEvaluationCriteria() async {
  final token = await storage.read(key: 'accessToken');
  final url = Uri.parse(AppConfig.CriteriaAndItems);

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
  final Project project;

  const EvaluateProjectPage({Key? key, required this.project})
      : super(key: key);

  @override
  _EvaluateProjectPageState createState() => _EvaluateProjectPageState();
}

class _EvaluateProjectPageState extends State<EvaluateProjectPage> {
  bool commitmentCheckbox = false;
  bool confirmEvaluationCheckbox = false;
  bool technicalMeritsSwitch = false;
  bool projectDesignSwitch = false;

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
      body: FutureBuilder(
        future: fetchEvaluationCriteria(),
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(child: Text("Error: ${snapshot.error}"));
          } else {
            final data = snapshot.data as Map<String, dynamic>?;

            final evaluationCriteria = data?['evaluationCriteriaDatas'] ?? [];
            final checklistItems = data?['checklistItemDatas'] ?? [];

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
                          style: TextStyle(
                              fontSize: 18, fontWeight: FontWeight.bold),
                        ),
                        SizedBox(height: 10),
                        const Text("Project Description",
                            style: TextStyle(fontWeight: FontWeight.bold)),
                        Text(widget.project.description),
                        SizedBox(height: 10),
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
                                  children: [
                                    Icon(Icons.circle,
                                        size: 8, color: Color(0xFF00B7FF)),
                                    const SizedBox(width: 4),
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
                        SizedBox(height: 14),
                        const Text("Team Members",
                            style: TextStyle(fontWeight: FontWeight.bold)),
                        Text("Kiran, Ege - 2000004002"),
                        Text("Kiran, Ege - 2000004002"),
                        Text("Kiran, Ege - 2000004002"),
                        SizedBox(height: 10),
                        CheckboxListTile(
                          activeColor: AppColors.primary,
                          value: commitmentCheckbox,
                          onChanged: (bool? value) {
                            setState(() {
                              commitmentCheckbox = value ?? false;
                            });
                          },
                          title:
                              const Text("Commitment to Impartial Evaluation"),
                          controlAffinity: ListTileControlAffinity.leading,
                        ),
                        Divider(height: 20, thickness: 1),
                        const Text(
                          "Part I - Evaluation Project Graduation Form",
                          style: TextStyle(
                              fontWeight: FontWeight.bold, fontSize: 16),
                        ),
                        const SizedBox(height: 14),
                        ...evaluationCriteria.map<Widget>((criteria) {
                          return Column(
                            children: [
                              Row(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
                                children: [
                                  Expanded(
                                    child: Text(
                                      criteria['criteriaName'],
                                      style: TextStyle(fontSize: 16),
                                    ),
                                  ),
                                  SizedBox(
                                    width: 60,
                                    height: 40,
                                    child: TextField(
                                      decoration: InputDecoration(
                                        border: OutlineInputBorder(),
                                      ),
                                      keyboardType: TextInputType.number,
                                    ),
                                  ),
                                ],
                              ),
                              TextField(
                                decoration: const InputDecoration(
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
                            ],
                          );
                        }).toList(),
                        Divider(height: 20, thickness: 1),
                        const Text(
                          "Part II - Graduation Project Checklist",
                          style: TextStyle(
                              fontWeight: FontWeight.bold, fontSize: 16),
                        ),
                        ...checklistItems.map<Widget>((item) {
                          return Column(
                            children: [
                              Row(
                                children: [
                                  Checkbox(value: false, onChanged: (value) {}),
                                  Expanded(
                                    child: Text(
                                      item['itemName'],
                                      style: TextStyle(fontSize: 16),
                                    ),
                                  ),
                                ],
                              ),
                              TextField(
                                decoration: const InputDecoration(
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
                        CheckboxListTile(
                          activeColor: AppColors.primary,
                          value: confirmEvaluationCheckbox,
                          onChanged: (bool? value) {
                            setState(() {
                              confirmEvaluationCheckbox = value ?? false;
                            });
                          },
                          title: const Text(
                            "I confirm my evaluation results.",
                            style: TextStyle(
                                fontWeight: FontWeight.bold, fontSize: 16),
                          ),
                          controlAffinity: ListTileControlAffinity.leading,
                        ),
                        SizedBox(height: 10),
                        ElevatedButton(
                          onPressed: confirmEvaluationCheckbox ? () {} : null,
                          style: ElevatedButton.styleFrom(
                            backgroundColor: AppColors.primary,
                            minimumSize: Size(double.infinity, 50),
                          ),
                          child: Text(
                            "Confirm",
                            style: TextStyle(fontWeight: FontWeight.bold),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            );
          }
        },
      ),
    );
  }
}

class CommitmentCheckbox extends StatelessWidget {
  final bool value;
  final ValueChanged<bool?> onChanged;

  const CommitmentCheckbox({
    required this.value,
    required this.onChanged,
  });

  @override
  Widget build(BuildContext context) {
    return CheckboxListTile(
      activeColor: AppColors.primary,
      value: value,
      onChanged: onChanged,
      title: const Text("Commitment to Impartial Evaluation"),
      controlAffinity: ListTileControlAffinity.leading,
    );
  }
}
