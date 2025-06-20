Improved versions of the default UI elements delivered by unity. Ability to use percentages and make a responsive design more easily.

# Elements
## Horizontal Layout Group (percent spacing)
- Currently only supports screen space calculations.
- Padding is a percent value.
  - The checkboxes toggle between screen space percentage (toggled on) and world space (parent) percentage (toggled off). Not implemented at this time.
- Spacing is a percent value. Only World space currently.

## Horizontal Layout Size Fitter
- Width Percent
  - Sets the width of the element in percent.
- Mode
  - Screen mode calculates it based on your whole screen width.
  - World mode calculates it based on the parent's width.
    - World mode does not work when the parent has a Content size fitter with a Horizontal fit other than unconstrained.
