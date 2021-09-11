/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable @typescript-eslint/ban-types */
/* eslint-disable jsx-a11y/label-has-associated-control */
/* eslint-disable max-len */
import React, { forwardRef, InputHTMLAttributes, useRef } from 'react';
// eslint-disable-next-line import/no-extraneous-dependencies
import classNames from 'classnames';
import './Input.styles.scss';

type DefaultInputProps = Omit<React.DetailedHTMLProps<React.InputHTMLAttributes<HTMLInputElement>, HTMLInputElement>, 'ref'>;

export type InputProps = {
    label?: string
    labelPosition?: string,
    hasError?: boolean
} & DefaultInputProps;

const Input: React.ForwardRefRenderFunction<HTMLInputElement | null, InputProps> = (props: InputProps, ref) => {
    const {
        label, labelPosition, hasError, disabled, className, ...rest
    } = props;

    const inputRef = useRef<HTMLInputElement>(null);
    const baseClass = 'pm-input';
    const classes = classNames({
        [baseClass]: true,
        [`${baseClass}--error`]: hasError,
        [`${baseClass}--disabled`]: hasError,
        className,
    });

    return (
        <div className={classes}>
            {labelPosition === 'left' && <label>{label}</label>}
            <input
                ref={(input) => {
                    (ref as Function)?.(input);
                    (inputRef as any).current = input;
                }}
                disabled={disabled}
                {...rest}
            />
            {labelPosition === 'right' && <label>{label}</label>}

        </div>
    );
};

export default forwardRef(Input);
