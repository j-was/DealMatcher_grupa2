import 'package:flutter/material.dart';
import 'package:frontend/Services/auth_service.dart';
import 'package:go_router/go_router.dart';

class MainAppBar extends StatelessWidget implements PreferredSizeWidget {
  const MainAppBar({super.key});

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);

  Future<void> _logout(BuildContext context) async {
    await AuthService.instance.clearSession();
    if (!context.mounted) {
      return;
    }
    context.go('/login');
  }

  @override
  Widget build(BuildContext context) {
    return AppBar(
      title: const Text('DealMatcher'),
      actions: [
        AnimatedBuilder(
          animation: AuthService.instance,
          builder: (context, _) {
            final isLoggedIn = AuthService.instance.isAuthenticated;

            return Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                IconButton(
                  tooltip: isLoggedIn ? 'Mój profil' : 'Zaloguj się',
                  onPressed: () {
                    context.go(isLoggedIn ? '/profile' : '/login');
                  },
                  icon: Icon(isLoggedIn ? Icons.person : Icons.login),
                ),
                if (isLoggedIn) ...[
                  IconButton(
                    tooltip: 'Wyloguj',
                    onPressed: () => _logout(context),
                    icon: const Icon(Icons.logout),
                  ),
                  IconButton(
                    tooltip: 'Dodaj ofertę',
                    onPressed: () {
                      context.go('/offer');
                    },
                    icon: const Icon(Icons.add),
                  ),
                ],
              ],
            );
          },
        ),
      ],
    );
  }
}
