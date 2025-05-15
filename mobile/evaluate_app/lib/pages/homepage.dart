import 'package:flutter/material.dart';
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:evaluate_app/models/models.dart';
import 'package:evaluate_app/pages/evaluate_project.dart';
import 'package:evaluate_app/pages/view_project_results.dart';
import 'package:evaluate_app/data/data_provider.dart';

class HomeScreen extends StatefulWidget {
  @override
  _HomeScreenState createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  List<Project> projects = [];
  bool isLoading = true;
  final DataProvider dataProvider = DataProvider();

  @override
  void initState() {
    super.initState();
    fetchProjects();
  }

  Future<void> fetchProjects() async {
    try {
      await dataProvider.fetchProjects();
      setState(() {
        projects = dataProvider.projects;
        isLoading = false;
      });
    } catch (error) {
      print('Error: $error');
      setState(() {
        isLoading = false;
      });
    }
  }

  void approveProject(int teamId) async {
    final result = await dataProvider.approveProject(teamId);
    if (result) {
      fetchProjects();
    } else {
      print('Failed to approve team with ID $teamId');
    }
  }

  void rejectProject(int teamId) async {
    final result = await dataProvider.rejectProject(teamId);
    if (result) {
      fetchProjects();
    } else {
      print('Failed to reject team with ID $teamId');
    }
  }

  void showConfirmationDialog(BuildContext context, String action, int teamId) {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext dialogContext) {
        return AlertDialog(
          backgroundColor: AppColors.whiteTextColor,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(20),
          ),
          title: const Text(
            'Confirm Action?',
            style: TextStyle(
              fontFamily: 'Inter',
              fontWeight: FontWeight.bold,
              fontSize: 20,
              color: AppColors.primaryTextColor,
            ),
          ),
          content: Text(
            'Are you sure you want to $action this team? This action cannot be undone.',
            style: const TextStyle(
              fontFamily: 'Inter',
              fontSize: 16,
              color: AppColors.primaryTextColor,
            ),
          ),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(dialogContext).pop();
              },
              child: const Text(
                'Cancel',
                style: TextStyle(
                  color: AppColors.primaryTextColor,
                  fontSize: 16,
                ),
              ),
            ),
            ElevatedButton(
              onPressed: () {
                Navigator.of(dialogContext).pop();
                if (action == 'approve') {
                  approveProject(teamId);
                } else {
                  rejectProject(teamId);
                }
              },
              style: ElevatedButton.styleFrom(
                backgroundColor:
                    action == 'approve' ? Colors.green : Colors.red,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(20),
                ),
              ),
              child: Text(
                action == 'approve' ? 'Approve' : 'Reject',
                style: const TextStyle(color: Colors.white),
              ),
            ),
          ],
        );
      },
    );
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
        toolbarHeight: 60,
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
              padding: const EdgeInsets.fromLTRB(16.0, 0, 16.0, 0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const SizedBox(
                    height: 10,
                  ),
                  const Text(
                    'Project Teams',
                    style: TextStyle(
                      color: AppColors.primaryTextColor,
                      fontFamily: 'Inter',
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(
                    height: 10,
                  ),
                  Expanded(
                    child: ListView.builder(
                      itemCount: projects.length,
                      itemBuilder: (context, index) {
                        final project = projects[index];
                        return buildTeamCard(
                          context,
                          project,
                          project.teamName,
                          project.projectName,
                          project.isApproval,
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
    Project project,
    String teamName,
    String projectName,
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
          height: 220,
          padding: const EdgeInsets.all(13.0),
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
              const Spacer(),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  if (!isApproval) ...[
                    IconButton(
                      onPressed: () {
                        showConfirmationDialog(
                            context, 'approve', project.teamId);
                      },
                      icon: const Icon(Icons.check_circle),
                      color: Colors.green,
                      iconSize: 40,
                    ),
                    IconButton(
                      onPressed: () {
                        showConfirmationDialog(
                            context, 'reject', project.teamId);
                      },
                      icon: const Icon(Icons.cancel),
                      color: Colors.red,
                      iconSize: 40,
                    ),
                  ] else ...[
                    ElevatedButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) =>
                                ViewProjectResults(project: project),
                          ),
                        );
                      },
                      style: ElevatedButton.styleFrom(
                        backgroundColor: AppColors.primary,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(20),
                        ),
                      ),
                      child: const Text(
                        'View Result',
                        style: TextStyle(color: Colors.white),
                      ),
                    ),
                    ElevatedButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) =>
                                EvaluateProjectPage(project: project),
                          ),
                        );
                      },
                      style: ElevatedButton.styleFrom(
                        backgroundColor: AppColors.primary,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(20),
                        ),
                      ),
                      child: const Text(
                        'Evaluate',
                        style: TextStyle(color: Colors.white),
                      ),
                    ),
                  ]
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
