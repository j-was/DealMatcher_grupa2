import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/login_form.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';

class LoginPage extends StatelessWidget {
  const LoginPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      body: const Padding(
        padding: EdgeInsetsGeometry.all(24),
        child: LoginForm(),
      ),
    );
  }
}
