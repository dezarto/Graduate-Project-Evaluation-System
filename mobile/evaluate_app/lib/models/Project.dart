// ignore_for_file: file_names

class Project {
  final String title;

  Project({
    required this.title,
  });

  factory Project.fromJson(Map<String, dynamic> json) {
    return Project(
      title: json['transactionId'],
    );
  }
}
