import 'package:flutter/material.dart';
import 'package:frontend/Models/offer.dart';
import 'package:frontend/Presentation/Widgets/main_app_bar.dart';
import 'package:frontend/Presentation/Widgets/main_side_menu.dart';
import 'package:frontend/Presentation/Widgets/reaction_buttons.dart';
import 'package:frontend/Presentation/Widgets/searched_offer_section.dart';

class SearchedOffersPage extends StatefulWidget {
  final List<Offer> offers;
  const SearchedOffersPage({super.key, required this.offers});
  @override
  State<SearchedOffersPage> createState() => _SearchedOffersPageState();
}

class _SearchedOffersPageState extends State<SearchedOffersPage> {
  int currIdx = 0;

  void nextOffer() {
    if (currIdx < widget.offers.length) {
      setState(() {
        currIdx++;
      });
    }
  }

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
            if (currIdx < widget.offers.length)
              Expanded(
                child: SearchedOfferSection(offer: widget.offers[currIdx]),
              )
            else
              Expanded(
                child: Center(
                  child: Text(
                    'Brak ofert',
                    style: const TextStyle(color: Colors.white54, fontSize: 32),
                  ),
                ),
              ),
            ReactionButtons(onDislike: nextOffer),
          ],
        ),
      ),
    );
  }
}
