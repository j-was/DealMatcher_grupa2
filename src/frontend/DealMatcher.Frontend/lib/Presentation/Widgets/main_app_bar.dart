import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

class MainAppBar extends StatelessWidget implements PreferredSizeWidget {
  const MainAppBar({super.key});

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);

  @override
  Widget build(BuildContext context) {
    return AppBar(
      title: Text('DealMatcher'),
      actions: [
        IconButton(
          onPressed: () {
            context.go('/offer');
          },
          icon: Icon(Icons.add),
        ),
      ],
    );
  }
}
