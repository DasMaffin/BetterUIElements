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

## Scroll Bounce
Automatically bounces a Scroll Rect between its min and max position.
- Speed
  - The amount of bounces (direction changes) per second. 1 = 1 bounce per second.
- Pause Time
  - The time bouncing is paused for when the user interacts with the Scroll rect.
- Horizontal
- Vertical
  - Both enable or disable bouncing in their respective directions.

## Better Image
An image that has rounded corners. No need for a "Background" sprite to get a panel anymore. Makes the rounded corners more flexible.
- Use percentages
  - Toggles the corner radius to use percentages or fixed values.
- Corner radius
  - The radius of the corner in pixels.
- Corner radius percent
  - The radius of the corner as a percentage of the shorter side of the rect transform.
- Segments
  - The amount of segments to be used to draw the corner.
  - Minimum 1.