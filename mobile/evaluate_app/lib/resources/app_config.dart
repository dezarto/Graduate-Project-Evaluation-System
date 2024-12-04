class AppConfig {
  // Base API URL
  static const String baseUrl = 'https://10.0.2.2:7107/api';

  // API Endpoints

  // GET Login
  static const String loginCats = '$baseUrl/LoginCats';

  // GET Project Teams
  static const String projectTeamView =
      '$baseUrl/Professor/get-project-team-view';

  // GET Profile Details
  static const String profileView = '$baseUrl/Professor/get-my-profile';

  // GET Checklist Items
  static const String checklistItems = '$baseUrl/Admin/get-all-checklist-items';

  // GET Evaluation Criterias
  static const String evaluationCriterias =
      '$baseUrl/Admin/get-all-evaluation-criteria';

  // GET Project Results
  static const String projectResult =
      '$baseUrl/Professor/get-project-team-result/{teamId}';

  // POST Submit Evaluation
  static const String submitEvaluation =
      '$baseUrl/Professor/get-project-team-result/{teamId}';

  // POST Approve Team
  static const String approveProject = '$baseUrl/Professor/post-approval-teams';
}
