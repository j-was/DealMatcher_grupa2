import 'package:flutter/material.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:go_router/go_router.dart';

class MainSideMenu extends StatelessWidget {
  const MainSideMenu({super.key});

  Future<void> _logout(BuildContext context) async {
    await AuthService.instance.clearSession();
    if (!context.mounted) {
      return;
    }
    context.pop();
    context.go('/login');
  }

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: AnimatedBuilder(
        animation: AuthService.instance,
        builder: (context, _) {
          final auth = AuthService.instance;
          final isLoggedIn = auth.isAuthenticated;
          final user = auth.currentUser;

          return ListView(
            padding: const EdgeInsets.only(top: 24),
            children: [
              DrawerHeader(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    const Icon(Icons.account_circle, size: 48),
                    const SizedBox(height: 12),
                    Text(
                      isLoggedIn && user != null
                          ? '${user.name} ${user.surname}'
                          : 'Gość',
                      style: const TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 4),
                    Text(
                      isLoggedIn && user != null
                          ? user.email
                          : 'Zaloguj się, aby zarządzać kontem',
                    ),
                  ],
                ),
              ),
              if (!isLoggedIn) ...[
                ListTile(
                  leading: const Icon(Icons.person),
                  title: const Text("Zarejestruj się"),
                  onTap: () {
                    context.pop();
                    context.go('/register');
                  },
                ),
                ListTile(
                  leading: const Icon(Icons.login),
                  title: const Text('Zaloguj się'),
                  onTap: () {
                    context.pop();
                    context.go('/login');
                  },
                ),
              ] else ...[
                ListTile(
                  leading: const Icon(Icons.person),
                  title: const Text('Mój profil'),
                  onTap: () {
                    context.pop();
                    context.go('/profile');
                  },
                ),
                ListTile(
                  leading: const Icon(Icons.logout),
                  title: const Text('Wyloguj'),
                  onTap: () => _logout(context),
                ),
              ],
              const ListTile(
                leading: Icon(Icons.play_arrow),
                title: Text('Przeglądaj'),
              ),
              const ListTile(
                leading: Icon(Icons.shopping_cart),
                title: Text('Koszyk'),
              ),
              ListTile(
                leading: Icon(Icons.search),
                title: Text('Szukaj'),
                onTap: () {
                  context.pop();
                  context.go('/search');
                },
              ),
              const ListTile(
                leading: Icon(Icons.settings),
                title: Text('Ustawienia'),
              ),
            ],
          );
        },
      ),
    );
  }
}
