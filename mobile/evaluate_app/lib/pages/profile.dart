import 'package:evaluate_app/pages/login_page.dart';
import 'package:flutter/material.dart';
import '../resources/app_resources.dart';
import 'package:evaluate_app/models/models.dart';

// User verisi
User user = User(
  professorId: 2,
  fullName: "Akhan Akbulut",
  department: "CSE",
  mailAddress: "a.akbulut@iku.edu.tr",
  role: "Professor",
);

class ProfilePage extends StatelessWidget {
  const ProfilePage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Profile'),
        backgroundColor: AppColors.primary,
        elevation: 0,
        centerTitle: false,
        titleTextStyle: const TextStyle(
          color: AppColors.whiteTextColor,
          fontFamily: 'Inter',
          fontSize: 30,
          fontWeight: FontWeight.bold,
        ),
      ),
      body: Container(
        decoration: BoxDecoration(color: AppColors.pageBackground),
        padding: const EdgeInsets.all(20),
        child: Center(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              // Profil Fotoğrafı
              Container(
                width: 500,
                padding: const EdgeInsets.symmetric(
                    horizontal: 20.0, vertical: 20.0), // Text etrafına padding

                decoration: BoxDecoration(
                  color: AppColors.whiteTextColor,
                  borderRadius:
                      BorderRadius.circular(10.0), // Kenarları yuvarlatmak için
                  border: Border.all(
                    color: const Color.fromARGB(
                        255, 201, 201, 201), // Kenarlık rengi
                    width: 0.7, // Kenarlık kalınlığı
                  ),
                ),
                child: Column(
                  children: [
                    CircleAvatar(
                      radius: 50,
                      backgroundImage: NetworkImage(
                          'https://www.example.com/your-profile-image.jpg'), // Profil fotoğrafı URL
                      backgroundColor: Colors.blueGrey[100],
                    ),
                    const SizedBox(height: 5),

                    // Ad Soyad
                    Text(
                      user.fullName,
                      style: const TextStyle(
                        fontSize: 24,
                        fontWeight: FontWeight.bold,
                      ),
                    ),

                    // Rol
                    Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 8.0,
                          vertical: 4.0), // Text etrafına padding
                      decoration: BoxDecoration(
                        color: Colors.amber,
                        borderRadius: BorderRadius.circular(
                            6.0), // Kenarları yuvarlatmak için
                      ),
                      child: Text(
                        user.role,
                        style: const TextStyle(
                          fontSize: 14,
                          fontWeight: FontWeight.bold,
                          color: AppColors.primaryTextColor,
                        ),
                      ),
                    ),
                  ],
                ),
              ),

              // Bölüm
              _ProfileInfoTile(
                label: 'Department',
                value: user.department,
              ),

              // E-posta
              _ProfileInfoTile(
                label: 'E-mail',
                value: user.mailAddress,
              ),

              // Professor ID
              _ProfileInfoTile(
                label: 'Professor ID',
                value: user.professorId.toString(),
              ),

              // Sign Out Button
              ElevatedButton(
                onPressed: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (context) => LoginPage(),
                    ),
                  );
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.primary, // Butonun arka plan rengi
                  padding: const EdgeInsets.symmetric(
                      horizontal: 80, vertical: 16), // Butonun iç paddingi
                  shape: RoundedRectangleBorder(
                    borderRadius:
                        BorderRadius.circular(30), // Kenarları yuvarlatmak için
                  ),
                ),
                child: const Text(
                  "Sign Out",
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: AppColors.whiteTextColor,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _ProfileInfoTile extends StatelessWidget {
  final String label;
  final String value;

  const _ProfileInfoTile({
    required this.label,
    required this.value,
  });

  @override
  Widget build(BuildContext context) {
    return ListTile(
      title: Text(
        label,
        style: const TextStyle(fontSize: 18, fontWeight: FontWeight.w500),
      ),
      subtitle: Text(
        value,
        style: const TextStyle(fontSize: 16),
      ),
      contentPadding: EdgeInsets.zero,
    );
  }
}
