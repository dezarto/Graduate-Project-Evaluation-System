class EvaluationCriteria {
  int criteriaId;
  bool isChecked;
  int? score;
  String? feedback;

  EvaluationCriteria({
    required this.criteriaId,
    this.isChecked = false,
    this.score,
    this.feedback,
  });
}

class ChecklistItem {
  int itemId;
  bool isChecked;
  String? feedback;

  ChecklistItem({
    required this.itemId,
    this.isChecked = false,
    this.feedback,
  });
}
