using System;

namespace XnaGui {
    /// <summary>
    ///     Represents the base class for controls that define a range of values.
    /// </summary>
    public abstract class RangeControlBase : Control {
        private int _maximum = 100;
        private int _minimum;
        private int _value;

        protected RangeControlBase(Control parent, int x, int y, int width, int height) : base(parent, x, y, width, height) {
        }

        /// <summary>
        ///     Gets or sets the maximum value of the control's range.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value was less than the minimum value of the range.</exception>
        public int Maximum {
            get => _maximum;
            set {
                if (value < _minimum) {
                    throw new ArgumentOutOfRangeException(nameof(value), "Maximum must not eclipse the minimum value of the range.");
                }

                _maximum = value;
            }
        }

        /// <summary>
        ///     Gets or sets the minimum value of the control's range.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value exceeded the maximum value.</exception>
        public int Minimum {
            get => _minimum;
            set {
                if (value > _maximum) {
                    throw new ArgumentOutOfRangeException(nameof(value), "Minimum must not exceed the maximum value of the range.");
                }

                _minimum = value;
            }
        }

        /// <summary>
        ///     Gets or sets the current position of the control, relative to its minimum and maximum values.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value was not within the defined range.</exception>
        public int Value {
            get => _value;
            set {
                if (value < _minimum) {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must not ellipse the minimum value of the range.");
                }

                if (value > _maximum) {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must not exceed the maximum value of the range.");
                }

                _value = value;
            }
        }
    }
}