import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';
import 'package:frontend/Presentation/Widgets/reaction_buttons.dart';

class MainPage extends StatelessWidget {
  const MainPage({super.key});
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      drawer: const MainSideMenu(),
      body: ReactionButtons(),
    );
  }
}
