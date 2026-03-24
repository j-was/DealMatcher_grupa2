import 'package:flutter/material.dart';

class DislikeButton extends StatelessWidget {
  const DislikeButton({super.key});
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: 60,
      height: 60,
      child: IconButton(
        onPressed: () => {},
        icon: Icon(Icons.close, color: const Color.fromARGB(255, 196, 51, 40)),
        iconSize: 50,
        padding: EdgeInsets.zero,
      ),
    );
  }
}
