import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';
import 'package:evaluate_app/resources/app_resources.dart';
import 'package:evaluate_app/mainwrapper.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class LoginPage extends StatefulWidget {
  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  bool _isPasswordVisible = false;
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final _formKey = GlobalKey<FormState>();

  // API login request function
  Future<void> _loginApiRequest(String username, String password) async {
    final Uri apiUrl = Uri.parse(AppConfig.loginCats);
    final storage = const FlutterSecureStorage();

    try {
      final response = await http
          .post(
            apiUrl,
            headers: <String, String>{
              'Content-Type': 'application/json; charset=UTF-8',
            },
            body: jsonEncode(<String, String>{
              'username': username,
              'password': password,
            }),
          )
          .timeout(const Duration(seconds: 80));

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseData = json.decode(response.body);
        print('Login successful: ${responseData['token']}');
        await storage.write(key: 'accessToken', value: responseData['token']);
        print('User login successfully completed! ');
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => MainWrapper()),
        );
      } else {
        print('Failed to login: ${response.request}');
        showDialog(
          context: context,
          builder: (BuildContext context) {
            return AlertDialog(
              title: const Text(
                'Bunun için üzgünüz!',
                style: TextStyle(
                  fontFamily: 'Lexend',
                  fontWeight: FontWeight.bold,
                ),
              ),
              content: const Text(
                'Sistemsel bir hatadan dolayı giriş yapılamadı. Lütfen bilgilerinizi kontrol edin ve tekrar deneyin.\nİnternet bağlantınızın olduğundan emin olun.',
                style: TextStyle(fontFamily: 'Lexend', fontSize: 15),
              ),
              actions: <Widget>[
                TextButton(
                  child: const Text('Tamam'),
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                ),
              ],
            );
          },
        );
        setState(() {});
      }
    } catch (error) {
      print('${username}  ${password}');
      print(apiUrl);
      print('Error occurred: $error'); // Log the error
      _showErrorSnackBar('An error occurred. Please try again later.');
    }
  }

  void _signIn() {
    if (_formKey.currentState?.validate() ?? false) {
      String email = _emailController.text;
      String password = _passwordController.text;

      _showLoadingDialog();

      Future.delayed(Duration(seconds: 3), () {
        Navigator.pop(context);

        // Call the API login request
        _loginApiRequest(email, password);
      });
    }
  }

  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        backgroundColor: AppColors.falseRed,
        content: Text(
          message,
          style: TextStyle(
            color: Colors.white,
            fontFamily: 'Inter',
          ),
        ),
      ),
    );
  }

  void _showLoadingDialog() {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) {
        return Center(
          child: Material(
            color: Colors.transparent,
            child: Container(
              width: 150.0,
              height: 150.0,
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(15.0),
              ),
              child: Center(
                child: CircularProgressIndicator(
                  color: AppColors.primary,
                ),
              ),
            ),
          ),
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: true,
      backgroundColor: AppColors.pageBackground,
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 24.0),
          child: Form(
            key: _formKey,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                SizedBox(height: 150.0),
                SvgPicture.asset(AppAssets.evaluateRedLogo, width: 280),
                SizedBox(height: 70.0),
                _buildEmailField(),
                SizedBox(height: 20.0),
                _buildPasswordField(),
                SizedBox(height: 20.0),
                _buildSignInButton(),
                SizedBox(height: 120.0),
                SvgPicture.asset(AppAssets.IKULogo, width: 130),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildEmailField() {
    return Container(
      height: AppDimens.inputTextFieldHeight,
      decoration: _inputDecoration(),
      child: TextFormField(
        controller: _emailController,
        decoration: InputDecoration(
          prefixIcon:
              Icon(Icons.email_outlined, color: AppColors.primaryTextColor),
          hintText: "Email",
          hintStyle: AppTextStyles.hintText,
          border: InputBorder.none,
          contentPadding: EdgeInsets.all(10.0),
          isDense: true, // Ensures compact layout for the input field
        ),
        style: AppTextStyles.inputText,
      ),
    );
  }

  Widget _buildPasswordField() {
    return Container(
      height: AppDimens.inputTextFieldHeight,
      decoration: _inputDecoration(),
      child: TextFormField(
        controller: _passwordController,
        obscureText: !_isPasswordVisible,
        decoration: InputDecoration(
          prefixIcon:
              Icon(Icons.lock_outline, color: AppColors.primaryTextColor),
          suffixIcon: IconButton(
            icon: Icon(
              _isPasswordVisible
                  ? Icons.visibility_outlined
                  : Icons.visibility_off_outlined,
              color: AppColors.primaryTextColor,
            ),
            onPressed: () =>
                setState(() => _isPasswordVisible = !_isPasswordVisible),
          ),
          hintText: "Password",
          hintStyle: AppTextStyles.hintText,
          border: InputBorder.none,
          contentPadding: EdgeInsets.all(10.0),
          isDense: true, // Ensures compact layout for the input field
        ),
        style: AppTextStyles.inputText,
      ),
    );
  }

  Widget _buildSignInButton() {
    return Container(
      width: double.infinity,
      height: AppDimens.signInButtonHeight,
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          backgroundColor: AppColors.primary,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(50.0),
          ),
        ),
        onPressed: _signIn,
        child: Text(
          "Sign In",
          style: AppTextStyles.buttonText,
        ),
      ),
    );
  }

  BoxDecoration _inputDecoration() {
    return BoxDecoration(
      color: AppColors.pageBackground,
      borderRadius: BorderRadius.circular(50.0),
      border: Border.all(color: AppColors.primaryTextColor, width: 1),
    );
  }
}
