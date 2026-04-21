import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/profile_form.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';

class ProfilePage extends StatelessWidget {
  const ProfilePage({super.key});

  @override
  Widget build(BuildContext context) {
    return const Scaffold(
      appBar: MainAppBar(),
      drawer: MainSideMenu(),
      body: Padding(padding: EdgeInsets.all(24), child: ProfileForm()),
    );
  }
}
