import 'package:evaluate_app/pages/calendar_page.dart';
import 'package:evaluate_app/pages/homepage.dart';
import 'package:evaluate_app/pages/profile.dart';
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:evaluate_app/widgets/home_navigation.dart';
import 'package:evaluate_app/widgets/calendar_navigation.dart';
import 'package:evaluate_app/widgets/profile_navigation.dart';

class MainWrapper extends StatefulWidget {
  const MainWrapper({super.key});

  @override
  MainWrapperState createState() => MainWrapperState();
}

class MainWrapperState extends State<MainWrapper> {
  int _selectedIndex = 1;

  final List<GlobalKey<NavigatorState>> _navigatorKeys = [
    profileNavigatorKey,
    homeNavigatorKey,
    calendarNavigatorKey,
  ];

  Future<bool> _systemBackButtonPressed() async {
    if (_navigatorKeys[_selectedIndex].currentState?.canPop() == true) {
      _navigatorKeys[_selectedIndex]
          .currentState
          ?.pop(_navigatorKeys[_selectedIndex].currentContext);
      return false;
    } else {
      SystemChannels.platform.invokeMethod<void>('SystemNavigator.pop');
      return true; // Indicate that the back action is handled
    }
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: _systemBackButtonPressed,
      child: Scaffold(
        bottomNavigationBar: NavigationBar(
          indicatorColor: AppColors.primary,
          backgroundColor: AppColors.whiteTextColor,
          onDestinationSelected: (int index) {
            setState(() {
              _selectedIndex = index;
            });
          },
          selectedIndex: _selectedIndex,
          destinations: [
            NavigationDestination(
              selectedIcon: Icon(
                Icons.calendar_today,
                color: _selectedIndex == 0
                    ? Colors.white
                    : AppColors.primaryTextColor,
              ),
              icon: Icon(
                Icons.calendar_today,
                color: AppColors.primaryTextColor,
              ),
              label: '', // Etiket boş bırakılarak gizlenir
            ),
            NavigationDestination(
              selectedIcon: Icon(
                Icons.home_filled,
                color: _selectedIndex == 1
                    ? Colors.white
                    : AppColors.primaryTextColor,
              ),
              icon: Icon(
                Icons.home_filled,
                color: AppColors.primaryTextColor,
              ),
              label: '',
            ),
            NavigationDestination(
              selectedIcon: Icon(
                Icons.person,
                color: _selectedIndex == 2
                    ? Colors.white
                    : AppColors.primaryTextColor,
              ),
              icon: Icon(
                Icons.person,
                color: AppColors.primaryTextColor,
              ),
              label: '',
            ),
          ],
        ),
        body: SafeArea(
          top: false,
          child: IndexedStack(
            index: _selectedIndex,
            children: <Widget>[
              CalendarScreen(),
              HomeScreen(),
              ProfilePage(),
            ],
          ),
        ),
      ),
    );
  }
}
