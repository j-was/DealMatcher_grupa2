import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';
import 'package:frontend/Presentation/Widgets/my_offers_section.dart';

class MyOffersPage extends StatelessWidget {
  const MyOffersPage({super.key});

  @override
  Widget build(BuildContext context) {
    return const Scaffold(
      appBar: MainAppBar(),
      drawer: MainSideMenu(),
      body: Padding(padding: EdgeInsets.all(24), child: MyOffersSection()),
    );
  }
}
