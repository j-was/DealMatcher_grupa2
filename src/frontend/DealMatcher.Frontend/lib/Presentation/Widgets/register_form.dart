import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/seperated_widget.dart';
import 'package:go_router/go_router.dart';

class RegisterForm extends StatefulWidget {
  const RegisterForm({super.key});

  @override
  State<RegisterForm> createState() => RegisterFormState();
}

class RegisterFormState extends State<RegisterForm> {
  final _nameController = TextEditingController();
  final _surnameController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();

  final _formKey = GlobalKey<FormState>();

  @override
  void dispose() {
    _nameController.dispose();
    _surnameController.dispose();
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
              key: const Key('nameField'),
              controller: _nameController,
              decoration: InputDecoration(labelText: 'Imię'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagane imię";
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('lastNameField'),
              controller: _surnameController,
              decoration: InputDecoration(labelText: 'Nazwisko'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagane nazwisko";
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('emailField'),
              controller: _emailController,
              decoration: InputDecoration(labelText: 'Email'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              keyboardType: TextInputType.emailAddress,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagany email";
                }
                if (!value.contains('@')) {
                  return 'Invalid email';
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('passwordField'),
              controller: _passwordController,
              decoration: InputDecoration(labelText: 'Hasło'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              obscureText: true,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagane hasło";
                }
                if (value.length < 4 || value.length > 32) {
                  return 'Hasło musi zawierać 4-32 znaków';
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          SeparatedWidget(
            widget: TextFormField(
              key: const Key('confirmPasswordField'),
              decoration: InputDecoration(labelText: 'Potwierdź hasło'),
              autovalidateMode: AutovalidateMode.onUserInteraction,
              obscureText: true,
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "Wymagane potwierdzenie hasła";
                }
                if (value != _passwordController.text) {
                  return "Hasła nie są identyczne";
                }
                return null;
              },
              style: TextStyle(color: Colors.white54),
            ),
          ),
          Center(
            child: SizedBox(
              width: 120,
              child: ElevatedButton(
                key: const Key('signUpSubmitButton'),
                onPressed: () async {
                  if (!(_formKey.currentState?.validate() ?? false)) {
                    return;
                  }
                  context.pop();
                  context.go('/');
                },
                child: Text(
                  'Wyślij',
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
