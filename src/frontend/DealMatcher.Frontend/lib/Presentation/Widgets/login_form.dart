import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/seperated_widget.dart';
import 'package:go_router/go_router.dart';

class LoginForm extends StatefulWidget {
  const LoginForm({super.key});

  @override
  State<LoginForm> createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _formKey = GlobalKey<FormState>();

  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    super.dispose();
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
                onPressed: () {
                  if (!(_formKey.currentState?.validate() ?? false)) {
                    return;
                  }

                  context.go('/');
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
