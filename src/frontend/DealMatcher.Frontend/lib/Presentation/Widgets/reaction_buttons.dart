import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/dislike_button.dart';
import 'package:frontend/Presentation/Widgets/like_button.dart';
import 'package:frontend/Presentation/Widgets/superlike_button.dart';

class ReactionButtons extends StatelessWidget {
  const ReactionButtons({super.key});
  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Row(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [DislikeButton(), SuperlikeButton(), LikeButton()],
      ),
    );
  }
}
