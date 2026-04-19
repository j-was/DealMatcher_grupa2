import 'package:flutter/material.dart';

class ProfileAvatarHeader extends StatelessWidget {
  final String name;
  final String surname;

  const ProfileAvatarHeader({
    super.key,
    required this.name,
    required this.surname,
  });

  String get _initials {
    final n = name.trim();
    final s = surname.trim();
    final first = n.isNotEmpty ? n[0].toUpperCase() : '';
    final second = s.isNotEmpty ? s[0].toUpperCase() : '';
    return '$first$second';
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        const SizedBox(height: 24),
        CircleAvatar(
          radius: 44,
          backgroundColor: Theme.of(context).primaryColor,
          child: Text(
            _initials,
            style: const TextStyle(
              fontSize: 32,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
        ),
        const SizedBox(height: 12),
        Text(
          '$name $surname',
          style: Theme.of(
            context,
          ).textTheme.titleLarge?.copyWith(color: Colors.white, fontSize: 20),
          textAlign: TextAlign.center,
        ),
        const SizedBox(height: 4),
        Text(
          'Mój profil',
          style: TextStyle(color: Colors.white54, fontSize: 13),
        ),
        const SizedBox(height: 24),
      ],
    );
  }
}
