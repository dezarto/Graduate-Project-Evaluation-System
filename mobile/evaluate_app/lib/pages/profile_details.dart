import 'package:flutter/material.dart';

class ProfileDetails extends StatelessWidget {
  const ProfileDetails({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Container(
        color: Colors.orange, // Sabit renk buradan ayarlanır.
        child: const Center(
          child: Text(
            'Sabit Renkli Sayfa',
            style: TextStyle(
              fontSize: 24,
              color: Colors.white, // Yazı rengi
              fontWeight: FontWeight.bold,
            ),
          ),
        ),
      ),
    );
  }
}
