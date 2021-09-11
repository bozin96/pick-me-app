/* eslint-disable react/jsx-props-no-spreading */
import React, { useMemo } from 'react';
import { FieldRenderProps } from 'react-final-form';
import Input from '../Input';

type Props = FieldRenderProps<string, any>;

const FieldInput: React.FC<Props> = ({ input, meta, ...rest }: Props) => {
    const { touched, error } = meta;
    const hasError = useMemo(() => touched && error, [touched, error]);
    return (
        <Input
            hasError={hasError}
            value={input.value}
            onChange={input.onChange}
            {...rest}
        />
    );
};

export default FieldInput;
