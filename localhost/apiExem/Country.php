<?php
/**
 * Created by PhpStorm.
 * User: Teacher
 * Date: 25.11.2018
 * Time: 12:39
 */

class Country
{
    public $id;
    public $countryName;
    public function __construct($id, $countryName)
    {
        $this->id = $id;
        $this->countryName = $countryName;
    }

    public function __call($name, $arguments)
    {
        // TODO: Implement __call() method.
        return 'Ты дебил!! Такого свойства нет';
    }

}