import 'package:flutter/material.dart';

class LikeButton extends StatelessWidget {
  const LikeButton({super.key});
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: 60,
      height: 60,
      child: IconButton(
        onPressed: () => {},
        icon: Icon(
          Icons.favorite_rounded,
          color: const Color.fromARGB(255, 39, 230, 45),
        ),
        iconSize: 40,
        padding: EdgeInsets.zero,
      ),
    );
  }
}
