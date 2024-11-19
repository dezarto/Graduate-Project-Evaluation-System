class Project {
  int teamId;
  int teamPresentationId;
  String teamName;
  String projectName;
  String description;
  bool isEvaluated;
  String evaluatingTeacherFullName;
  String evaluatingTeacherMail;
  List<Student> studentsList;

  Project({
    required this.teamId,
    required this.teamPresentationId,
    required this.teamName,
    required this.projectName,
    required this.description,
    required this.isEvaluated,
    required this.evaluatingTeacherFullName,
    required this.evaluatingTeacherMail,
    required this.studentsList,
  });

  factory Project.fromJson(Map<String, dynamic> json) {
    return Project(
      teamId: json['teamId'],
      teamPresentationId: json['teamPresentationId'],
      teamName: json['teamName'],
      projectName: json['projectName'],
      description: json['description'],
      isEvaluated: json['isEvaluated'],
      evaluatingTeacherFullName: json['evaluatingTeacherFullName'],
      evaluatingTeacherMail: json['evaluatingTeacherMail'],
      studentsList: (json['studentsList'] as List)
          .map((student) => Student.fromJson(student))
          .toList(),
    );
  }
}

class Student {
  int studentId;
  String studentFullName;
  String studentNumber;

  Student({
    required this.studentId,
    required this.studentFullName,
    required this.studentNumber,
  });

  factory Student.fromJson(Map<String, dynamic> json) {
    return Student(
      studentId: json['studentId'],
      studentFullName: json['studentFullName'],
      studentNumber: json['studentNumber'],
    );
  }
}
