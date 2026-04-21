import 'package:flutter/material.dart';

class SuperlikeButton extends StatelessWidget {
  final VoidCallback? onPressed;
  const SuperlikeButton({super.key, this.onPressed});
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: 60,
      height: 60,
      child: IconButton(
        onPressed: onPressed ?? () {},
        icon: Icon(
          Icons.star_rounded,
          color: const Color.fromARGB(255, 250, 210, 12),
        ),
        iconSize: 45,
        padding: EdgeInsets.zero,
      ),
    );
  }
}
