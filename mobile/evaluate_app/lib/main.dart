import 'package:evaluate_app/pages/login_page.dart';
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:flutter/material.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'dart:io';
import 'package:evaluate_app/data/data_provider.dart';
import 'package:provider/provider.dart';

void main() {
  WidgetsBinding widgetsBinding = WidgetsFlutterBinding.ensureInitialized();
  FlutterNativeSplash.preserve(widgetsBinding: widgetsBinding);

  HttpOverrides.global = MyHttpOverrides();

  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (_) => DataProvider()),
      ],
      child: const MyApp(),
    ),
  );
}

class MyApp extends StatefulWidget {
  const MyApp({super.key});

  @override
  State<MyApp> createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
  @override
  void initState() {
    super.initState();
    initialization();
  }

  void initialization() async {
    print('pausing...');
    await Future.delayed(const Duration(seconds: 3));
    print('unpausing');
    FlutterNativeSplash.remove();
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: ThemeData(useMaterial3: true, primaryColor: AppColors.primary),
      home: LoginPage(),
    );
  }
}

// Bu https istek gönderirken ssl sertifikasını ignorelamak için eklendi
// http istek atacaksan bunu silmen gerekebilir ama sanmıyorum
// http istek atacaksan http://10.2.2.2:5054 e istek atıcaksın
// android studio local hostu 10.2.2.2 de görüyor kg -mög
class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)
      ..badCertificateCallback =
          (X509Certificate cert, String host, int port) => true;
  }
}
