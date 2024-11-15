import 'package:flutter/material.dart';
import 'package:evaluate_app/pages/homepage.dart';
import 'package:evaluate_app/pages/login_page.dart';

class Home extends StatefulWidget {
  const Home({super.key});

  @override
  HomeState createState() => HomeState();
}

GlobalKey<NavigatorState> homeNavigatorKey = GlobalKey<NavigatorState>();

class HomeState extends State<Home> {
  @override
  Widget build(BuildContext context) {
    return Navigator(
      key: homeNavigatorKey,
      onGenerateRoute: (RouteSettings settings) {
        return MaterialPageRoute(
            settings: settings,
            builder: (BuildContext context) {
              if (settings.name == "/detailsHome") {
                return LoginPage();
              }
              return HomeScreen();
            });
      },
    );
  }
}
