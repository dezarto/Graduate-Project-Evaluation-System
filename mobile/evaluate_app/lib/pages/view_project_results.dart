import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import '../resources/app_resources.dart';
import 'package:evaluate_app/models/models.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class ViewProjectResults extends StatefulWidget {
  final Project project;

  const ViewProjectResults({Key? key, required this.project}) : super(key: key);

  @override
  _ViewProjectResultsState createState() => _ViewProjectResultsState();
}

class _ViewProjectResultsState extends State<ViewProjectResults> {
  final storage = const FlutterSecureStorage();
  late Future<Map<String, dynamic>> evaluationDataFuture;

  @override
  void initState() {
    super.initState();
    evaluationDataFuture = fetchEvaluationCriteria();
  }

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
        return json.decode(response.body);
      } else {
        throw Exception('Failed to load evaluation criteria: ${response.body}');
      }
    } catch (error) {
      print('Error fetching evaluation criteria: $error');
      return {};
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
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
          icon: const Icon(Icons.arrow_back),
          onPressed: () => Navigator.of(context).pop(),
        ),
      ),
      body: FutureBuilder<Map<String, dynamic>>(
        future: evaluationDataFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(child: Text('Error: ${snapshot.error}'));
          } else if (!snapshot.hasData || snapshot.data == null) {
            return const Center(child: Text('No data available'));
          }

          final data = snapshot.data!;
          final evaluationCriteria =
              data['evaluationCriteriaDatas'] as List<dynamic>? ?? [];
          final checklistItems =
              data['checklistItemDatas'] as List<dynamic>? ?? [];

          return SingleChildScrollView(
            child: Container(
              margin: const EdgeInsets.all(16.0),
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(16),
                border: Border.all(
                  color: const Color.fromARGB(255, 201, 201, 201),
                  width: 0.7,
                ),
              ),
              padding: const EdgeInsets.all(16.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    widget.project.projectName,
                    style: const TextStyle(
                        fontSize: 18, fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 10),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text("Evaluating Teacher",
                              style: TextStyle(fontWeight: FontWeight.bold)),
                          Text(widget.project.evaluatingTeacherFullName),
                        ],
                      ),
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text("Status",
                              style: TextStyle(fontWeight: FontWeight.bold)),
                          Row(
                            children: const [
                              Icon(Icons.circle,
                                  size: 8, color: Color(0xFF0DBF00)),
                              SizedBox(width: 4),
                              Text(
                                "Result Available",
                                style: TextStyle(
                                    color: Color(0xFF0DBF00),
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
                  const SizedBox(height: 20),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: const [
                      Text("Criteria",
                          style: TextStyle(
                              fontWeight: FontWeight.bold, fontSize: 16)),
                      Text("Grade",
                          style: TextStyle(
                              fontWeight: FontWeight.bold, fontSize: 16)),
                    ],
                  ),
                  const Divider(thickness: 1),
                  ...evaluationCriteria.map((criteria) {
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
                            SizedBox(
                              width: 50,
                              child: Text(
                                criteria['score'].toString(),
                                style: const TextStyle(fontSize: 16),
                                textAlign: TextAlign.center,
                              ),
                            ),
                          ],
                        ),
                        const Divider(thickness: 1),
                      ],
                    );
                  }).toList(),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text("Total Score (%100)",
                          style: TextStyle(
                              fontWeight: FontWeight.bold, fontSize: 16)),
                      Text(
                        data['totalScore'].toString(),
                        style: const TextStyle(
                            fontWeight: FontWeight.bold, fontSize: 16),
                      ),
                    ],
                  ),
                  const SizedBox(height: 20),
                  const Text("General Feedback",
                      style:
                          TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                  Text(
                    data['generalComments'] ?? "No feedback provided",
                    style: const TextStyle(fontSize: 16),
                  ),
                  const SizedBox(height: 20),
                  const Text("Checklist Items",
                      style:
                          TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                  const Divider(thickness: 1),
                  ...checklistItems.map((item) {
                    return Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          children: [
                            const Icon(Icons.check_circle, color: Colors.green),
                            const SizedBox(width: 8),
                            Expanded(
                              child: Text(
                                item['itemName'],
                                style: const TextStyle(fontSize: 16),
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 8),
                      ],
                    );
                  }).toList(),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}
