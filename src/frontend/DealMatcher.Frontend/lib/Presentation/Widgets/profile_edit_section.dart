import 'package:flutter/material.dart';

class ProfileEditSection extends StatelessWidget {
  final TextEditingController nameController;
  final TextEditingController surnameController;

  const ProfileEditSection({
    super.key,
    required this.nameController,
    required this.surnameController,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.only(bottom: 8),
          child: Text(
            'Edytuj profil',
            style: TextStyle(
              color: Theme.of(context).primaryColor,
              fontSize: 12,
              fontWeight: FontWeight.w600,
              letterSpacing: 0.8,
            ),
          ),
        ),
        TextFormField(
          key: const Key('profileNameField'),
          controller: nameController,
          decoration: const InputDecoration(
            labelText: 'Imię',
            prefixIcon: Icon(Icons.person_outline, color: Colors.white38),
          ),
          autovalidateMode: AutovalidateMode.onUserInteraction,
          validator: (value) {
            if (value == null || value.trim().isEmpty) {
              return 'Wymagane imię';
            }
            return null;
          },
          style: const TextStyle(color: Colors.white),
          textInputAction: TextInputAction.next,
        ),
        const SizedBox(height: 12),
        TextFormField(
          key: const Key('profileSurnameField'),
          controller: surnameController,
          decoration: const InputDecoration(
            labelText: 'Nazwisko',
            prefixIcon: Icon(Icons.person_outline, color: Colors.white38),
          ),
          autovalidateMode: AutovalidateMode.onUserInteraction,
          validator: (value) {
            if (value == null || value.trim().isEmpty) {
              return 'Wymagane nazwisko';
            }
            return null;
          },
          style: const TextStyle(color: Colors.white),
          textInputAction: TextInputAction.done,
        ),
      ],
    );
  }
}
