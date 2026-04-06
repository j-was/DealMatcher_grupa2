import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/offer_form.dart';

class AddOfferPage extends StatelessWidget {
  const AddOfferPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      body: Padding(padding: EdgeInsetsGeometry.all(24), child: AddOfferForm()),
    );
  }
}
