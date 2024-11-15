import 'package:flutter/material.dart';
import '../resources/app_resources.dart';

class EvaluateProjectPage extends StatefulWidget {
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
        backgroundColor: AppColors.primary,
        elevation: 0,
        centerTitle: false,
        titleTextStyle: const TextStyle(
          color: AppColors.whiteTextColor,
          fontFamily: 'Inter',
          fontSize: 24,
          fontWeight: FontWeight.bold,
        ),
        title: Text("Team Name 1"),
        leading: IconButton(
          color: AppColors.whiteTextColor,
          icon: Icon(Icons.arrow_back),
          onPressed: () => Navigator.of(context).pop(),
        ),
      ),
      body: Container(
        decoration: BoxDecoration(color: AppColors.pageBackground),
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Container(
            decoration: BoxDecoration(
                color: Colors.white, borderRadius: BorderRadius.circular(20)),
            child: Container(
              padding: const EdgeInsets.all(13.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    "AI Supported Human Resources System\nfor IT Companies",
                    style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                  ),
                  SizedBox(height: 10),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text("Evaluating Teacher",
                              style: TextStyle(fontWeight: FontWeight.bold)),
                          Text("a.akbulut@iku.edu.tr"),
                        ],
                      ),
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          const Text("Status",
                              style: TextStyle(fontWeight: FontWeight.bold)),
                          Row(
                            children: [
                              Icon(Icons.circle, size: 8, color: Colors.blue),
                              const SizedBox(width: 4),
                              Text(
                                "Ready to Evaluate",
                                style: TextStyle(
                                    color: Colors.blue,
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
                  Text("Team Name 1"),
                  SizedBox(height: 14),
                  const Text("Team Members",
                      style: TextStyle(fontWeight: FontWeight.bold)),
                  Text("Kiran, Ege - 2000004002"),
                  Text("Kiran, Ege - 2000004002"),
                  Text("Kiran, Ege - 2000004002"),
                  SizedBox(height: 20),
                  CheckboxListTile(
                    value: commitmentCheckbox,
                    onChanged: (bool? value) {
                      setState(() {
                        commitmentCheckbox = value ?? false;
                      });
                    },
                    title: const Text("Commitment to Impartial Evaluation"),
                    controlAffinity: ListTileControlAffinity.leading,
                  ),
                  Divider(height: 20, thickness: 1),
                  const Text(
                    "Part I - Evaluation Project Graduation Form",
                    style: TextStyle(fontWeight: FontWeight.bold),
                  ),
                  CheckboxListTile(
                    value: technicalMeritsSwitch,
                    onChanged: (bool? value) {
                      setState(() {
                        technicalMeritsSwitch = value ?? false;
                      });
                    },
                    title: Row(
                      mainAxisAlignment: MainAxisAlignment.start,
                      children: [
                        const Text("Technical Merits (%15)"),
                      ],
                    ),
                  ),
                  TextField(
                    decoration: InputDecoration(
                      hintText: "Write your thoughts...",
                      border: OutlineInputBorder(),
                    ),
                    maxLines: 3,
                  ),
                  SizedBox(height: 10),
                  CheckboxListTile(
                    value: projectDesignSwitch,
                    onChanged: (bool? value) {
                      setState(() {
                        projectDesignSwitch = value ?? false;
                      });
                    },
                    title: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        const Text("Project Design and Implementation (%40)"),
                      ],
                    ),
                  ),
                  TextField(
                    decoration: InputDecoration(
                      hintText: "Write your thoughts...",
                      border: OutlineInputBorder(),
                    ),
                    maxLines: 3,
                  ),
                  Divider(height: 20, thickness: 1),
                  const Text(
                    "Part II - Graduation Project Checklist",
                    style: TextStyle(fontWeight: FontWeight.bold),
                  ),
                  CheckboxListTile(
                    value: false,
                    onChanged: (bool? value) {},
                    title: const Text(
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit."),
                    controlAffinity: ListTileControlAffinity.leading,
                  ),
                  TextField(
                    decoration: InputDecoration(
                      hintText: "Write your thoughts...",
                      border: OutlineInputBorder(),
                    ),
                    maxLines: 3,
                  ),
                  SizedBox(height: 10),
                  CheckboxListTile(
                    value: false,
                    onChanged: (bool? value) {},
                    title: const Text(
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit."),
                    controlAffinity: ListTileControlAffinity.leading,
                  ),
                  TextField(
                    decoration: InputDecoration(
                      hintText: "Write your thoughts...",
                      border: OutlineInputBorder(),
                    ),
                    maxLines: 3,
                  ),
                  SizedBox(height: 20),
                  CheckboxListTile(
                    value: confirmEvaluationCheckbox,
                    onChanged: (bool? value) {
                      setState(() {
                        confirmEvaluationCheckbox = value ?? false;
                      });
                    },
                    title: const Text("I confirm my evaluation results."),
                    controlAffinity: ListTileControlAffinity.leading,
                  ),
                  SizedBox(height: 20),
                  ElevatedButton(
                    onPressed: () {},
                    style: ElevatedButton.styleFrom(
                      backgroundColor: AppColors.primary,
                      padding: EdgeInsets.symmetric(vertical: 16),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(10),
                      ),
                    ),
                    child: Center(
                      child: const Text(
                        "Evaluate",
                        style: TextStyle(
                            fontSize: 18, color: AppColors.whiteTextColor),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
