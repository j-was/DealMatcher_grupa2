import 'package:flutter/material.dart';
import 'package:frontend/Models/user_login_data.dart';
import 'package:frontend/Presentation/Widgets/seperated_widget.dart';
import 'package:frontend/Services/user_service.dart';
import 'package:go_router/go_router.dart';
import 'package:frontend/Services/auth_service.dart';

class LoginForm extends StatefulWidget {
  const LoginForm({super.key});

  @override
  State createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  final _userService = UserService();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _formKey = GlobalKey<FormState>();

  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  String _extractMessage(Object error) {
    return error
        .toString()
        .replaceFirst('Exception: ', '')
        .replaceFirst('Bad state: ', '');
  }

  @override
  Widget build(BuildContext context) {
    return Form(
      key: _formKey,
      child: ListView(
        children: [
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('loginEmailField'),
              controller: _emailController,
              decoration: const InputDecoration(labelText: 'Email'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              keyboardType: TextInputType.emailAddress,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Wymagany email';
                }
                if (!value.contains('@')) {
                  return 'Niepoprawny email';
                }
                return null;
              },
              style: const TextStyle(color: Colors.white54),
            ),
          ),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('loginPasswordField'),
              controller: _passwordController,
              decoration: const InputDecoration(labelText: 'Hasło'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              obscureText: true,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return 'Wymagane hasło';
                }
                if (value.length < 4 || value.length > 32) {
                  return 'Hasło musi zawierać 4-32 znaków';
                }
                return null;
              },
              style: const TextStyle(color: Colors.white54),
            ),
          ),
          Center(
            child: SizedBox(
              width: 140,
              child: ElevatedButton(
                key: const Key('loginSubmitButton'),
                onPressed: () async {
                  if (!(_formKey.currentState?.validate() ?? false)) {
                    return;
                  }

                  try {
                    final loginUserData = UserLoginData(
                      email: _emailController.text.trim(),
                      password: _passwordController.text,
                    );

                    final response = await _userService.loginUser(
                      loginUserData,
                    );
                    await AuthService.instance.saveSession(response);

                    if (!context.mounted) {
                      return;
                    }

                    context.go('/profile');
                  } catch (e) {
                    if (!context.mounted) {
                      return;
                    }

                    ScaffoldMessenger.of(
                      context,
                    ).showSnackBar(SnackBar(content: Text(_extractMessage(e))));
                  }
                },
                child: const Text(
                  'Zaloguj',
                  style: TextStyle(fontSize: 18, color: Colors.white),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
