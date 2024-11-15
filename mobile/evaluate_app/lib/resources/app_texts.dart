import 'package:flutter/material.dart';
import 'package:evaluate_app/resources/app_resources.dart';

class AppTextStyles {
  static const TextStyle hintText = TextStyle(
    color: AppColors.secondaryTextColor,
    fontFamily: 'Inter',
    fontWeight: FontWeight.w400,
  );

  static const TextStyle inputText = TextStyle(
    color: AppColors.primaryTextColor,
    fontFamily: 'Inter',
    fontWeight: FontWeight.w400,
  );

  static const TextStyle buttonText = TextStyle(
    color: AppColors.whiteTextColor,
    fontFamily: 'Inter',
    fontWeight: FontWeight.w500,
    fontSize: 16.0,
  );

  static const TextStyle errorText = TextStyle(
    color: AppColors.falseRed,
    fontFamily: 'Inter',
    fontWeight: FontWeight.w500,
  );

  static const TextStyle labelText = TextStyle(
    color: AppColors.primaryTextColor,
    fontFamily: 'Inter',
    fontWeight: FontWeight.w600,
    fontSize: 14.0,
  );
}
