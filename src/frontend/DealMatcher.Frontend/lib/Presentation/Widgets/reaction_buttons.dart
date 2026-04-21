import 'package:flutter/material.dart';
import 'package:frontend/Presentation/Widgets/dislike_button.dart';
import 'package:frontend/Presentation/Widgets/like_button.dart';
import 'package:frontend/Presentation/Widgets/superlike_button.dart';

class ReactionButtons extends StatelessWidget {
  final VoidCallback onDislike;
  final VoidCallback? onLike;
  final VoidCallback? onSuperlike;

  const ReactionButtons({
    super.key,
    required this.onDislike,
    this.onLike,
    this.onSuperlike,
  });
  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        DislikeButton(onPressed: onDislike),
        SuperlikeButton(onPressed: onSuperlike),
        LikeButton(onPressed: onLike),
      ],
    );
  }
}
