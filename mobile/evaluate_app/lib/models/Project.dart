class Project {
  final int teamId;
  final int teamPresentationId;
  final String teamName;
  final String projectName;
  final String description;
  final bool isEvaluated;
  final bool isApproval;
  final String evaluatingTeacherFullName;
  final String evaluatingTeacherMail;
  final String presentationDate;
  final String startTime;
  final String endTime;
  final List<TeamMember> members; // Team members list

  Project({
    required this.teamId,
    required this.teamPresentationId,
    required this.teamName,
    required this.projectName,
    required this.description,
    required this.isEvaluated,
    required this.isApproval,
    required this.evaluatingTeacherFullName,
    required this.evaluatingTeacherMail,
    required this.presentationDate,
    required this.startTime,
    required this.endTime,
    required this.members,
  });

  factory Project.fromJson(Map<String, dynamic> json) {
    return Project(
      teamId: json['teamId'] ?? 0,
      teamPresentationId: json['teamPresentationId'] ?? 0,
      teamName: json['teamName'] ?? '',
      projectName: json['projectName'] ?? '',
      description: json['description'] ?? '',
      isEvaluated: json['isEvaluated'] ?? false,
      isApproval: json['isApproval'] ?? false,
      evaluatingTeacherFullName: json['evaluatingTeacherFullName'] ?? '',
      evaluatingTeacherMail: json['evaluatingTeacherMail'] ?? '',
      presentationDate: json['presentationDate'] ?? '',
      startTime: json['startTime'] ?? '',
      endTime: json['endTime'] ?? '',
      members: (json['members'] as List<dynamic>?)
              ?.map((memberJson) => TeamMember.fromJson(memberJson))
              .toList() ??
          [],
    );
  }
}

class TeamMember {
  final int studentId;
  final String fullName;
  final String studentNumber;

  TeamMember({
    required this.studentId,
    required this.fullName,
    required this.studentNumber,
  });

  factory TeamMember.fromJson(Map<String, dynamic> json) {
    return TeamMember(
      studentId: json['studentId'] ?? 0,
      fullName: json['studentFullName'] ?? '',
      studentNumber: json['studentNumber'] ?? '',
    );
  }
}
