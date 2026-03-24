import 'package:flutter/material.dart';

class MainSideMenu extends StatelessWidget {
  const MainSideMenu({super.key});

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        padding: EdgeInsets.only(top: 24),
        children: const [
          ListTile(leading: Icon(Icons.person), title: Text("Jan Kowalski")),
          ListTile(leading: Icon(Icons.play_arrow), title: Text('Browse')),
          ListTile(leading: Icon(Icons.shopping_cart), title: Text('Cart')),
          ListTile(leading: Icon(Icons.search), title: Text('Search')),
          ListTile(leading: Icon(Icons.settings), title: Text('Settings')),
        ],
      ),
    );
  }
}
