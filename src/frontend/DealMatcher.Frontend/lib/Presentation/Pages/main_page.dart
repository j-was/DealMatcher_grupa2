import 'package:flutter/material.dart';
import 'package:frontend/Models/category.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Models/seller.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
//import 'package:frontend/Presentation/Widgets/main_page_offer_section.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';
import 'package:frontend/Presentation/Widgets/offer_view.dart';
import 'package:frontend/Presentation/Widgets/reaction_buttons.dart';

class MainPage extends StatelessWidget {
  // Hardcoded example Offer for app testing.
  late final Offer offer = Offer(
    id: 1,
    title: "oferta",
    description:
        "opisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisopisv",
    price: 128.00,
    images: ["https://picsum.photos/seed/abc/800/600"],
    seller: Seller(id: 1, name: 'seller', rating: 21.37),
    category: Category(
      id: 2,
      name: 'category',
      description: 'category decription',
    ),
    tags: ['tag'],
    properties: [('propertyName', 'propertyValue')],
    availability: 10,
    status: 'ACTIVE',
    createdAt: DateTime.now(),
    updatedAt: DateTime.now(),
  );

  MainPage({super.key});
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const MainAppBar(),
      drawer: const MainSideMenu(),
      body: Padding(
        padding: EdgeInsetsGeometry.all(24),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Expanded(child: OfferView(offer: offer)),
            ReactionButtons(),
          ],
        ),
      ),
    );
  }
}
