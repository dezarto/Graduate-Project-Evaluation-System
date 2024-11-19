import 'package:flutter/material.dart';
import '../resources/app_resources.dart';

class ViewProjectResults extends StatelessWidget {
  const ViewProjectResults({Key? key}) : super(key: key);

  List<Widget> buildEvaluationCriteria() {
    return evaluationCriteria.map((criteria) {
      return Column(
        children: [
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 10.0),
            child: Row(
              children: [
                Expanded(
                  child: Text(
                    criteria,
                    style: const TextStyle(fontSize: 16),
                  ),
                ),
                const SizedBox(width: 14),
                SizedBox(
                  width: 50,
                  child: const Text(
                    '16',
                    style: TextStyle(fontSize: 16),
                  ),
                ),
              ],
            ),
          ),
          const Divider(thickness: 1),
        ],
      );
    }).toList();
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
        title: const Text("Team Name 1"),
        leading: IconButton(
          color: AppColors.whiteTextColor,
          icon: const Icon(Icons.arrow_back),
          onPressed: () => Navigator.of(context).pop(),
        ),
      ),
      body: Container(
        decoration: BoxDecoration(color: AppColors.pageBackground),
        child: SingleChildScrollView(
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
                const Text(
                  "AI Supported Human Resources System for IT Companies",
                  style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                ),
                const SizedBox(height: 10),
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: const [
                        Text("Evaluating Teacher",
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
                const Text("Team Name 1"),
                const SizedBox(height: 14),
                const Text("Team Members",
                    style: TextStyle(fontWeight: FontWeight.bold)),
                const Text("Kiran, Ege - 2000004002"),
                const Text("Kiran, Ege - 2000004002"),
                const Text("Kiran, Ege - 2000004002"),
                const SizedBox(height: 20),
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: const [
                    Text("Criteria",
                        style: TextStyle(
                            fontWeight: FontWeight.bold, fontSize: 16)),
                    SizedBox(
                      width: 65,
                      child: const Text(
                        'Grade', // Örnek not
                        style: TextStyle(
                            fontSize: 16, fontWeight: FontWeight.bold),
                      ),
                    ),
                  ],
                ),
                const Divider(thickness: 1),
                ...buildEvaluationCriteria(),
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: const [
                    Text("Total Score (%100)",
                        style: TextStyle(
                            fontWeight: FontWeight.bold, fontSize: 16)),
                    SizedBox(
                      width: 50,
                      child: const Text(
                        '16', // Örnek not
                        style: TextStyle(
                            fontSize: 16, fontWeight: FontWeight.bold),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 20),
                const Text("General Feedback",
                    style:
                        TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                const Text(
                    "The project was very good. alkdj alksdj kjsd kjdsskj ksjd ",
                    style: TextStyle(fontSize: 16)),
                const SizedBox(height: 20),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
