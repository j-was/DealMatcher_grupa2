import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:go_router/go_router.dart';

class AdminMenuPage extends StatelessWidget {
  const AdminMenuPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      body: Center(
        child: Padding(
          padding: const EdgeInsets.all(24),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              SizedBox(
                width: 240,
                height: 56,
                child: ElevatedButton.icon(
                  onPressed: () {
                    context.go('/admin/bans');
                  },
                  icon: const Icon(Icons.people, color: Colors.white54),
                  label: const Text(
                    'Zablokowani użytkownicy',
                    style: TextStyle(color: Colors.white54),
                  ),
                ),
              ),
              const SizedBox(height: 16),
              SizedBox(
                width: 240,
                height: 56,
                child: ElevatedButton.icon(
                  onPressed: () {
                    context.go('/admin/users');
                  },
                  icon: const Icon(Icons.people, color: Colors.white54),
                  label: const Text(
                    'Lista użytkowników',
                    style: TextStyle(color: Colors.white54),
                  ),
                ),
              ),
              const SizedBox(height: 16),
              SizedBox(
                width: 240,
                height: 56,
                child: ElevatedButton.icon(
                  onPressed: () {
                    context.go('/admin/offers');
                  },
                  icon: const Icon(Icons.local_offer, color: Colors.white54),
                  label: const Text(
                    'Lista ofert',
                    style: TextStyle(color: Colors.white54),
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
