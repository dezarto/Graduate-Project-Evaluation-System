import 'package:flutter/material.dart';
import '../resources/app_resources.dart';
import 'package:evaluate_app/pages/evaluate_project.dart';

class HomeScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Home'),
        backgroundColor: AppColors.primary,
        elevation: 0,
        centerTitle: false,
        titleTextStyle: const TextStyle(
          color: AppColors.whiteTextColor,
          fontFamily: 'Inter',
          fontSize: 30,
          fontWeight: FontWeight.bold,
        ),
      ),
      body: Container(
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
            const SizedBox(height: 16),
            Expanded(
              child: ListView(
                children: [
                  buildTeamCard(
                    context, // Pass context here
                    'Team Name 1',
                    'A really long project name for graduation project 1 aldjakdj alkdjsalkdj akj alksdks slk ksjdla sldj',
                    'Ready to Evaluate',
                    Colors.blue,
                    'Evaluate',
                  ),
                  buildTeamCard(
                    context, // Pass context here
                    'Team Name 2',
                    'A really long project name for graduation project 1',
                    'Ready to Evaluate',
                    Colors.blue,
                    'Evaluate',
                  ),
                  buildTeamCard(
                    context, // Pass context here
                    'Team Name 3',
                    'A really long project name for graduation project 1',
                    'Result Available',
                    Colors.green,
                    'View Result',
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget buildTeamCard(
    BuildContext context, // Receive context as a parameter
    String teamName,
    String projectName,
    String statusText,
    Color statusColor,
    String buttonText,
  ) {
    const int maxChars = 90; // Set the desired character limit

    // Truncate the project name and add "..." if it exceeds the limit
    String truncatedProjectName = projectName.length > maxChars
        ? projectName.substring(0, maxChars) + '...'
        : projectName;

    return Padding(
      padding: const EdgeInsets.only(bottom: 10.0),
      child: Card(
        color: Colors.white, // Pure white color for the card
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
              // First Column: Team and Project Info
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
              // Second Column: Status and Button
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
                    ElevatedButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) =>
                                EvaluateProjectPage(), // Replace with your page class name
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
