import 'package:flutter/material.dart';
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:evaluate_app/models/models.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:evaluate_app/pages/evaluate_project.dart';
import 'package:evaluate_app/pages/view_project_results.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class HomeScreen extends StatefulWidget {
  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  List<Project> projects = [];
  bool isLoading = true;
  final storage = const FlutterSecureStorage();

  @override
  void initState() {
    super.initState();
    fetchProjects();
  }

  Future<void> fetchProjects() async {
    final token = await storage.read(key: 'accessToken');
    final url = Uri.parse(AppConfig.projectTeamView);

    try {
      final response = await http.get(
        url,
        headers: {
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        print('${response.statusCode}: Projects fetched successfully!');
        final List<dynamic> data = json.decode(response.body);
        setState(() {
          projects = data.map((e) => Project.fromJson(e)).toList();
          isLoading = false;
        });
      } else {
        throw Exception('${response.statusCode}: Failed to load projects.');
      }
    } catch (error) {
      print('Error fetching projects: $error');
      setState(() {
        isLoading = false;
      });
    }
  }

  void approveProject(int teamId) async {
    final token = await storage.read(key: 'accessToken');
    final url =
        Uri.parse('${AppConfig.approveProject}?teamId=$teamId&approval=true');

    try {
      final response = await http.post(
        url,
        headers: {
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        print('Team with ID $teamId approved successfully!');
        fetchProjects();
      } else {
        throw Exception('Failed to approve team with ID $teamId.');
      }
    } catch (error) {
      print('Error approving team: $error');
    }
  }

  void rejectProject(int teamId) async {
    final token = await storage.read(key: 'accessToken');
    final url =
        Uri.parse('${AppConfig.approveProject}?teamId=$teamId&approval=false');

    try {
      final response = await http.post(
        url,
        headers: {
          'Authorization': 'Bearer $token',
        },
      );
      if (response.statusCode == 200) {
        print('Team with ID $teamId rejected successfully!');
        fetchProjects();
      } else {
        throw Exception('Failed to reject team with ID $teamId.');
      }
    } catch (error) {
      print('Error rejecting team: $error');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        scrolledUnderElevation: 0,
        automaticallyImplyLeading: false,
        title: const Text('Home'),
        backgroundColor: AppColors.primary,
        elevation: 0,
        centerTitle: false,
        titleTextStyle: const TextStyle(
          color: AppColors.whiteTextColor,
          fontFamily: 'Inter',
          fontSize: 36,
          fontWeight: FontWeight.bold,
        ),
        leading: null,
        toolbarHeight: 100,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.vertical(
            bottom: Radius.circular(20),
          ),
        ),
      ),
      body: isLoading
          ? const Center(child: CircularProgressIndicator())
          : Container(
              decoration: BoxDecoration(color: AppColors.pageBackground),
              padding: const EdgeInsets.all(16.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Project Teams',
                    style: TextStyle(
                      color: AppColors.primaryTextColor,
                      fontFamily: 'Inter',
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  Expanded(
                    child: ListView.builder(
                      itemCount: projects.length,
                      itemBuilder: (context, index) {
                        final project = projects[index];
                        return buildTeamCard(
                          context,
                          project.teamName,
                          project.projectName,
                          project.isEvaluated
                              ? 'Result Available'
                              : 'Ready to Evaluate',
                          project.isEvaluated ? Colors.green : Colors.blue,
                          project.isEvaluated ? 'View Result' : 'Evaluate',
                          project.isEvaluated,
                          !project.isApproval,
                        );
                      },
                    ),
                  ),
                ],
              ),
            ),
    );
  }

  Widget buildTeamCard(
    BuildContext context,
    String teamName,
    String projectName,
    String statusText,
    Color statusColor,
    String buttonText,
    bool isEvaluated,
    bool isApproval,
  ) {
    const int maxChars = 90;

    String truncatedProjectName = projectName.length > maxChars
        ? projectName.substring(0, maxChars) + '...'
        : projectName;

    return Padding(
      padding: const EdgeInsets.only(bottom: 8.0),
      child: Card(
        color: Colors.white,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(20),
        ),
        elevation: 3,
        shadowColor: Colors.black26,
        child: Container(
          height: 170,
          padding: const EdgeInsets.all(13.0),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.end,
            children: [
              Expanded(
                flex: 3,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text(
                      'Team',
                      style: TextStyle(
                        fontFamily: 'Inter',
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Text(
                      teamName,
                      style: const TextStyle(
                        fontSize: 14,
                        color: AppColors.primaryTextColor,
                      ),
                    ),
                    const SizedBox(height: 16),
                    const Text(
                      'Project Name',
                      style: TextStyle(
                        fontFamily: 'Inter',
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Text(
                      truncatedProjectName,
                      style: const TextStyle(
                        fontSize: 14,
                        color: AppColors.primaryTextColor,
                      ),
                    ),
                  ],
                ),
              ),
              Expanded(
                flex: 2,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.end,
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const Text(
                          'Status',
                          style: TextStyle(
                            fontFamily: 'Inter',
                            fontSize: 16,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        Row(
                          children: [
                            Icon(Icons.circle, size: 8, color: statusColor),
                            const SizedBox(width: 4),
                            Text(
                              statusText,
                              style: TextStyle(
                                fontSize: 14,
                                color: statusColor,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                    isEvaluated
                        ? ElevatedButton(
                            onPressed: () {
                              Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (context) => ViewProjectResults(),
                                ),
                              );
                            },
                            style: ElevatedButton.styleFrom(
                              backgroundColor: AppColors.primary,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(20),
                              ),
                            ),
                            child: Text(
                              buttonText,
                              style: const TextStyle(
                                color: Colors.white,
                              ),
                            ),
                          )
                        : isApproval
                            ? ElevatedButton(
                                onPressed: () {
                                  Navigator.push(
                                    context,
                                    MaterialPageRoute(
                                      builder: (context) =>
                                          EvaluateProjectPage(),
                                    ),
                                  );
                                },
                                style: ElevatedButton.styleFrom(
                                  backgroundColor: AppColors.primary,
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(20),
                                  ),
                                ),
                                child: Text(
                                  buttonText,
                                  style: const TextStyle(
                                    color: Colors.white,
                                  ),
                                ),
                              )
                            : Row(
                                mainAxisAlignment: MainAxisAlignment.end,
                                children: [
                                  IconButton(
                                    onPressed: () {
                                      // Onaylama işlemi
                                      print('Project approved');
                                    },
                                    icon: const Icon(Icons.check_circle),
                                    color: Colors.green,
                                    iconSize: 36,
                                  ),
                                  const SizedBox(width: 10),
                                  IconButton(
                                    onPressed: () {
                                      // Reddetme işlemi
                                      print('Project rejected');
                                    },
                                    icon: const Icon(Icons.cancel),
                                    color: Colors.red,
                                    iconSize: 36,
                                  ),
                                ],
                              ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
