import 'package:flutter/material.dart';

class SuperlikeButton extends StatelessWidget {
  const SuperlikeButton({super.key});
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: 60,
      height: 60,
      child: IconButton(
        onPressed: () => {},
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
