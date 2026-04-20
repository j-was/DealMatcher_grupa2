import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';
import 'package:frontend/Presentation/Widgets/offer_edit_form.dart';

class OfferEditPage extends StatelessWidget {
  final int offerId;

  const OfferEditPage({super.key, required this.offerId});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      drawer: const MainSideMenu(),
      body: Padding(
        padding: const EdgeInsets.all(24),
        child: OfferEditForm(offerId: offerId),
      ),
    );
  }
}
