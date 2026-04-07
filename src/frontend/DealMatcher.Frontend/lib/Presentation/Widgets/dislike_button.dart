import 'package:flutter/material.dart';

class DislikeButton extends StatelessWidget {
  final VoidCallback onPressed;

  const DislikeButton({super.key, required this.onPressed});
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: 60,
      height: 60,
      child: IconButton(
        onPressed: () => onPressed(),
        icon: Icon(Icons.close, color: const Color.fromARGB(255, 196, 51, 40)),
        iconSize: 50,
        padding: EdgeInsets.zero,
      ),
    );
  }
}
