import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

class MainSideMenu extends StatelessWidget {
  const MainSideMenu({super.key});

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        padding: EdgeInsets.only(top: 24),
        children: [
          ListTile(
            leading: Icon(Icons.person),
            title: Text("Zarejestruj się"),
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
          ListTile(leading: Icon(Icons.play_arrow), title: Text('Przeglądaj')),
          ListTile(leading: Icon(Icons.shopping_cart), title: Text('Koszyk')),
          ListTile(
            leading: Icon(Icons.search),
            title: Text('Szukaj'),
            onTap: () {
              context.pop();
              context.go('/search');
            },
          ),
          ListTile(leading: Icon(Icons.settings), title: Text('Ustawienia')),
        ],
      ),
    );
  }
}
