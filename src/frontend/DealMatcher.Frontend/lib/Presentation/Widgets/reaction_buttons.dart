import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/dislike_button.dart';
import 'package:frontend/Presentation/Widgets/like_button.dart';
import 'package:frontend/Presentation/Widgets/superlike_button.dart';

class ReactionButtons extends StatelessWidget {
  final VoidCallback onDislike;

  const ReactionButtons({super.key, required this.onDislike});
  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        DislikeButton(onPressed: onDislike),
        SuperlikeButton(),
        LikeButton(),
      ],
    );
  }
}
