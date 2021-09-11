/* eslint-disable react/button-has-type */
/* eslint-disable import/no-extraneous-dependencies */
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/ban-types */
/* eslint-disable react/jsx-props-no-spreading */
import classNames from 'classnames';
import React, { forwardRef, useRef } from 'react';
import './Button.styles.scss';

type DefaultButtonProps = React.DetailedHTMLProps<React.ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement>

type ButtonProps = {
    variant: 'primary' | 'outline' | 'secondary',
} & DefaultButtonProps;

const Button: React.ForwardRefRenderFunction<HTMLButtonElement | null,
    ButtonProps> = (props: ButtonProps, ref) => {
        const {
            children, className, variant = 'primary', type = 'button', disabled,
            ...rest
        } = props;

        const baseClass = 'pm-button';
        const classes = classNames(baseClass, `${baseClass}--${variant}`, className);

        const buttonRef = useRef<HTMLButtonElement>();

        return (
            <button
                type={type}
                className={classes}
                ref={(button) => {
                    (ref as Function)?.(button);
                    (buttonRef as any).current = button;
                }}
                disabled={disabled}
                {...rest}
            >
                {children}
            </button>
        );
    };

export default forwardRef(Button);
