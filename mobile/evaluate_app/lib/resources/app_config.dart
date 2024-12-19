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

  // GET Evaluation Criterias and Checklist Items
  static const String criteriaAndItems =
      '$baseUrl/Professor/get-evaluations-criteria-and-check-list-datas';

  // GET Project Results
  static const String projectResult =
      '$baseUrl/Professor/get-project-team-result/{teamId}';

  // POST Submit Evaluation
  static const String submitEvaluation = '$baseUrl/Professor/submit-evaluation';

  // POST Approve Team
  static const String approveProject = '$baseUrl/Professor/post-approval-teams';

  // POST Available Time
  static const String availableTime =
      '$baseUrl/Professor/post-availability-by-professor';

  // GET Available Time
  static const String getAvailTimes =
      '$baseUrl/Professor/get-availability-by-professor-auth';

  // DELETE Available Time
  static const String deleteAvailTimes =
      '$baseUrl/Professor/delete-availability-by-id';
}
